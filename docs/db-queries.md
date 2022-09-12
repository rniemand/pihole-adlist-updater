# DB Queries
Useful queries to make things a lot easier.

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
