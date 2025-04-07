ALTER TABLE ServiceBooking 
ADD 
    UpdatedAt DATETIME2 NULL,
    DeletedAt DATETIME2 NULL,
    CreatedBy INT NULL,
    UpdatedBy INT NULL,
    DeletedBy INT NULL,
    DeletedReason text NULL,
    IsDeleted BIT NOT NULL DEFAULT 0
