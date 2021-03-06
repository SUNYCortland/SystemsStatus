/* SS_WIDGET_SERVICES Table */
DECLARE iCount NUMBER;
BEGIN
	SELECT COUNT(*) INTO iCount FROM user_tables WHERE table_name = 'SS_WIDGET_SERVICES';
	IF (iCount = 0) THEN
		EXECUTE IMMEDIATE 'CREATE TABLE SS_WIDGET_SERVICES (Widget_Id NUMBER, Widget_Service_Id NUMBER)';
		EXECUTE IMMEDIATE 'CREATE UNIQUE INDEX SS_WIDGET_SERVICES_PK ON SS_WIDGET_SERVICES ("WIDGET_ID", "WIDGET_SERVICE_ID")';
		EXECUTE IMMEDIATE 'ALTER TABLE SS_WIDGET_SERVICES ADD CONSTRAINT SS_WIDGET_SERVICES_PK PRIMARY KEY ("WIDGET_ID", "WIDGET_SERVICE_ID")';
	END IF;
END;