
DECLARE @ID INT
DECLARE temp_cursor CURSOR FOR       
SELECT ID
FROM usermaster where subjectsinterested <>'' and subjectsinterested is not null
OPEN temp_cursor      
    
FETCH NEXT FROM temp_cursor       
INTO @ID
    
    
WHILE @@FETCH_STATUS = 0      
BEGIN   
 
Declare @val Varchar(MAX)=''; 
	Select @val = COALESCE(@val + ',' + cast(id AS NVARCHAR(max)), cast(id AS NVARCHAR(max))) 
        from categorymaster where name in (select value from StringSplit((select subjectsinterested      
		from usermaster where id = @ID and subjectsinterested <>'' and subjectsinterested is not null),','));
	
	update usermaster set subjectsinterested=right(@val,len(@val)-1) where id = @ID
FETCH NEXT FROM temp_cursor       
INTO @ID
     
END       
CLOSE temp_cursor;      
DEALLOCATE temp_cursor
