  SELECT CONCAT('                new Character
                {
                    CharacterId = ', CharacterId, ',
					',
                    'Name = "', Name, '",
					',
                    'ShortName = "',ShortName, '",
				},')
FROM [C:\C#\MESSOYAHAHSE\MHSER\APP_DATA\TESTDB.MDF].[dbo].[Characters]