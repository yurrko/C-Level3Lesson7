SELECT CONCAT('                new Category
                {
                    CategoryId = ', CategoryID, ',
					',
                    'Name = "', Name, '",
					',
                    'ShortName = "',ShortName, '",
				},')
FROM [C:\C#\MESSOYAHAHSE\MHSER\APP_DATA\TESTDB.MDF].[dbo].[Categories]