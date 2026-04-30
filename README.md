# Mooforest

## 課題管理
### データベース
#### Issues
* Id (Primary Key)
* Title (Non Null)
* Description
* StatusId (Foreign Key, Non Null)
* CreatedAt (Non Null)
* UpdatedAt (Non Null)
* ParentId
* ToDo

#### History
* Id (Primary Key)
* IssueId (Foreign Key)
* CreatedAt (Non Null)
* Description (Non Null)
* StatusId (Foreign Key, Non Null)

### Statuses
* Id (Primary Key)
* Name (Non Null, Unique)
* SortOrder (Non Null, Unique)
* IsClosed (Non Null, Boolean)