    SELECT CONCAT('                new User
                {
                    UserId = ', UserId, ',
					',
                    'Name = @"', Name, '",
					',
                    'Position = "',Position, '",
				},')
FROM [C:\C#\MESSOYAHAHSE\MHSER\APP_DATA\TESTDB.MDF].[dbo].[Users]
