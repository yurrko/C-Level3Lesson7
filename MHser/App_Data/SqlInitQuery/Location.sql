SELECT CONCAT('                new Location
                {
                    LocationId = ', LocationId, ',
					',
                    'Name = @"', Name, '",
				},')
FROM [TEST_d22b26948e5b44c9b5be9bf98fc435eb].[dbo].[Locations]