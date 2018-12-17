using Microsoft.EntityFrameworkCore.Migrations;

namespace SamuraiApp.Data.Migrations
{
    public partial class SQLToAddScalarFunction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                @"CREATE FUNCTION [dbo].[EarliestBattleFoughtBySamurai] (@samuraiId int)
                  RETURNS char(30) AS
                  BEGIN
                    DECLARE @ret char(30);
                    SELECT TOP 1 @ret = Name
                    FROM Battles
                    WHERE Battles.Id IN (SELECT BattleId
                                         FROM SamuraiBattle
                                         WHERE SamuraiId = @samuraiId)
                    ORDER BY Startdate
                    RETURN @ret;
                END");

            migrationBuilder.Sql(
                @"CREATE FUNCTION [dbo].[DaysInBattle] 
                  (
                    @startdate date,
                    @enddate date
                  )
                  RETURNS int AS
                  BEGIN
                    DECLARE @days int;

                    SELECT @days = datediff(D, @startdate, @enddate) + 1

                    RETURN @days;
                END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP FUNCTION [dbo].[EarliestBattleFoughtBySamurai]");
            migrationBuilder.Sql("DROP FUNCTION [dbo].[DaysInBattle]");
        }
    }
}
