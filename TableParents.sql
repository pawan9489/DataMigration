--SELECT * FROM SYS.foreign_keys

--select * from sys.tables



;with cte as (
select AllTables.name, 
(select name from sys.tables where object_id = Parents.referenced_object_id ) Parent from sys.tables AllTables
left join sys.foreign_keys Parents
on AllTables.object_id = Parents.parent_object_id 
) 
select distinct c1.name 'Table', Parents = STUFF(( SELECT	',' + Parent
				FROM	cte as c2
				WHERE c2.name = c1.name
				FOR XML	PATH('')
				), 1, 1, '')
from cte c1


