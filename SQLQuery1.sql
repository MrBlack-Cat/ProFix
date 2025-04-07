ALTER TABLE SubscriptionPlan
ADD 
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(), 
    UpdatedAt DATETIME2 NULL, 
    DeletedAt DATETIME2 NULL,
    CreatedBy INT NULL,
    UpdatedBy INT NULL,
    DeletedBy INT NULL,
    DeletedReason TEXT NULL,
    IsDeleted BIT NOT NULL DEFAULT 0;
