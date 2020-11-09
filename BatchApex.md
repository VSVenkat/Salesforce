
## Batch Apex

Batch Apex allows you to define a single job that can be broken up into manageble chunks , where every chunk can be processed separately

Eg : if you want to make a field update  of every record of Account object in your organization, then we have Governing limits that would restrict us from achieving the above task.

Reason : In a single transaction we can process only 10000 records, Now in the above case if we have more than 10000 records in the organization then we cannot perform this  field update .

Batch Apex : In the Batch Apex it will fetch all the records on which you want to perform the field update and divide them into list of 200 records  and on every 200 records  operation is performed separately. This will help us to execute on more than 10000 records as it won't perform the transaction on all the 10000 records in a single transaction instead it is divided into number of sub tasks where each subtask may contain records upto 200 records.

### Database.Batchable interface
To implement the Batch Apex concept Apex class must implement the Database.Batchable interface

1. Database.Batchable interface consists of 3 methods that must be implemented

- Start method
- Execute method
- Finish method

2. Start Method : 
- Start method is automatically called at the begining of the Batch Apex Job.
- It will collect the records or objects on which the operation has to be performed.
- These records are divided into sub tasks and given to execute method.

Syntax : 
```
global(Database.QueryLocator|Iterable<SObject>) start(Database.BatchableContext bd){}
 ```
 3. The return type of start method can be
 
 - Database.QueryLocator OR
 - Iterable<SObject>
  
 








