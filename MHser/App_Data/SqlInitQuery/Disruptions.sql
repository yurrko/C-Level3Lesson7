 SELECT CONCAT('                new Disruption
                {
                    DisruptionId = ', DisruptionId, ',
					',
                    'DetectionTime = DateTime.Parse("', DetectionTime, '"),
					',
					'ExecuteUntil = DateTime.Parse("', ExecuteUntil, '"),
					',
                    'Description = @"', Description, '",
					',
                    'Documentation = @"', Documentation, '",
					',
                    'UserId = ', UserId, ',
					',
                    'LocationId = ', LocationId, ',
					',
                    'CategoryId = ', CategoryId, ',
					',
                    'CharacterId = ', CharacterId, ',
					',
                    'ResponsableId = ', ResponsableId, ',
				},')
FROM [TEST_d22b26948e5b44c9b5be9bf98fc435eb].[dbo].[Disruptions]