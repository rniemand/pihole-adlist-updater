# DB Queries
Useful queries to make things a lot easier.

## AdLists

### Add New List
```sql
INSERT INTO `AdLists`
	(`AdListType`, `AdListSource`, `Maintainer`, `ProjectUrl`, `ListUrl`)
VALUES
  (3, 6, 'maintainer' , 'project_url' , 'list_url')
```

Please refer to [AdListSource](./enums/AdListSource.md) and [AdListType](./enums/AdListType.md) for possible values.

## Domain Queries

### Check for Enrty
```sql
SELECT d.*
FROM `Domains` d
WHERE
	d.`Domain` LIKE 'console.cloud.%'
```

## Whitelist Queries

### Add Exact
```sql
INSERT INTO `Whitelists`
  (`IsRegex`, `Order`, `Expression`)
VALUES
  (0, 255, 'connectivitycheck.gstatic.com');
```
