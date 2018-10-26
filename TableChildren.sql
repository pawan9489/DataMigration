SELECT DISTINCT object_name(fk.referenced_object_id) [Parent], 
    SUBSTRING(
        (
            SELECT ','+object_name(tmp.parent_object_id)  AS [text()]
            FROM sys.foreign_keys tmp
            WHERE tmp.referenced_object_id = fk.referenced_object_id
            ORDER BY tmp.referenced_object_id
            FOR XML PATH ('')
        ), 2, 1000) [Children]
FROM sys.foreign_keys fk


