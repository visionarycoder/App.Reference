CREATE TABLE dbo.RuleDefinitions
( 
  RuleId int identity primary key,
  Slice   nvarchar(50),   -- 'Discovery', 'Litigation', …
  Name    nvarchar(100),
  Json    nvarchar(max),
  Version int,
  IsActive bit,
  UpdatedUtc datetime2 
  
);