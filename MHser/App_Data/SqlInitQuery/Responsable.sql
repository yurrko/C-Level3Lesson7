SELECT CONCAT('                new Responsable
                {
                    ResponsableId = ', ResponsableId, ',
					',
                    'Name = @"', Name, '",
					',
                    'Position = "',Position, '",
					',
                    'LocationId = ', LocationId, ',
				},')
FROM [TEST_d22b26948e5b44c9b5be9bf98fc435eb].[dbo].[Responsables]