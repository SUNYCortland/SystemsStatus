/* SS_WIDGETS Table */
DECLARE iCount NUMBER;
BEGIN
	SELECT COUNT(*) INTO iCount FROM user_tables WHERE table_name = 'SS_WIDGETS';
	IF (iCount = 0) THEN
		EXECUTE IMMEDIATE 'CREATE TABLE SS_WIDGETS (Widget_Id NUMBER, Widget_User_Id NUMBER, Widget_Height NUMBER, Widget_Name VARCHAR2(200 BYTE), Widget_Department_Id NUMBER, Widget_Guid VARCHAR2(32 BYTE) DEFAULT SYS_GUID())';
		EXECUTE IMMEDIATE 'CREATE UNIQUE INDEX SS_WIDGETS_PK ON SS_WIDGETS ("WIDGET_ID")';
		EXECUTE IMMEDIATE 'ALTER TABLE SS_WIDGETS ADD CONSTRAINT SS_WIDGETS_PK PRIMARY KEY ("WIDGET_ID")';
	END IF;
END;