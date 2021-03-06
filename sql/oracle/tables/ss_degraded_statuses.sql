/* SS_DEGRADED_STATUSES Table */
DECLARE iCount NUMBER;
BEGIN
	SELECT COUNT(*) INTO iCount FROM user_tables WHERE table_name = 'SS_DEGRADED_STATUSES';
	IF (iCount = 0) THEN
		EXECUTE IMMEDIATE 'CREATE TABLE SS_DEGRADED_STATUSES (Status_Id NUMBER, Degraded_Status_Cause VARCHAR2(2000 BYTE), Degraded_Status_Degraded_Since DATE, Degraded_Status_Exp_Online DATE, Degraded_Status_Act_Online DATE)';
		EXECUTE IMMEDIATE 'CREATE UNIQUE INDEX SS_DEGRADED_STATUSES_PK ON SS_DEGRADED_STATUSES ("STATUS_ID")';
		EXECUTE IMMEDIATE 'ALTER TABLE SS_DEGRADED_STATUSES ADD CONSTRAINT SS_DEGRADED_STATUSES_PK PRIMARY KEY ("STATUS_ID")';
	END IF;
END;