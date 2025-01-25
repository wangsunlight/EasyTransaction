# 封装TransactionScope 实现事务

> 主要功能包括: 
> 1. 使用ActionFilter 对 Action 实现事务
> 2. 通过对DI进行动态代理实现对调用方法的事务

### TransactionScopeOption 对象有以下三个选项：

Required:联接环境事务，或者在环境事务不存在的情况下创建新的环境事务。

> 使用场景：当你希望方法在现有事务中执行，如果没有现有事务，则创建一个新事务。

Requires New:成为新的根范围，也就是说，启动一个新事务并使该事务成为其自己范围中的新环境事务。

> 使用场景：当你希望方法总是启动一个新事务，而不管是否存在现有事务。

Suppress:根本不参与事务。因此没有环境事务。

> 使用场景：当你希望方法不参与任何事务时。


| TransactionScopeOption | 是否存在环境事务 | 范围参与                   |
| ---------------------- | ---------------- | -------------------------- |
| Required               | 否               | 参与新事务（将成为根范围） |
| Requires New           | 否               | 参与新事务（将成为根范围） |
| Suppress               | 否               | 不参与任何事务             |
| Required               | 是               | 参与环境事务               |
| Requires New           | 是               | 参与新事务（将成为根范围） |
| Suppress               | 是               | 不参与任何事务             |


### TransactionOptions IsolationLevel 事务隔离级别有以下几种：

1. ReadUncommitted (System.Transactions.IsolationLevel.ReadUncommitted)
   未提交读（俗称“脏读”，能够读取其他用户正在修改的尚未提交的数据，无法确保数据的正确性）

   > 使用场景：适用于对数据一致性要求不高的场景，例如读取日志数据或统计数据时，可以容忍脏读。

2. ReadCommitted (System.Transactions.IsolationLevel.ReadCommitted)
   提交读（无法读取正在修改【未提交的】的数据，即：读取修改后的数据）

   > 使用场景：适用于大多数业务场景，确保读取的数据是已提交的，避免脏读。

3. RepeatableRead (System.Transactions.IsolationLevel.RepeatableRead)
   可重复读（无法修改正在读取【未提交的】的数据，即：读取修改后的数据）

   > 使用场景：适用于需要确保读取数据一致性的场景，例如生成报表时，确保读取的数据在整个事务期间不变。

4. Serializable (System.Transactions.IsolationLevel.Serializable)
   可串行读（最高隔离级别，一个事务未提交，另一个事务就会一直等待你提交数据）

   > 使用场景：适用于需要最高数据一致性的场景，例如银行转账，确保事务期间数据不被其他事务修改。

5. Snapshot (System.Transactions.IsolationLevel.Snapshot)
   快照读（事务在读取数据时会创建一个数据快照，确保读取的数据一致性）

   > 使用场景：适用于需要读取数据一致性但不希望锁定数据的场景，例如长时间运行的查询。

6. Chaos (System.Transactions.IsolationLevel.Chaos)
   混乱（不允许任何更低级别的事务修改数据）、

   > 使用场景：很少使用，适用于需要确保事务不会被更低级别的事务修改的特殊场景。

7. Unspecified (System.Transactions.IsolationLevel.Unspecified)
   未指定（使用默认的隔离级别）

   > 使用场景：使用默认的隔离级别，通常不推荐使用，除非明确知道默认隔离级别适合当前场景。