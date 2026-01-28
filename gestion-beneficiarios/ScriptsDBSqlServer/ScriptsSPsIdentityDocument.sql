CREATE PROCEDURE sp_IdentityDocument_GetAll
(
    @IsActive BIT = NULL
)
AS
BEGIN
    SET NOCOUNT ON;

    /*
        Descripción:
        Retorna el listado de documentos de identidad con todos sus datos como
        nombre del documento, abreviatura, país, longitud de carácteres, si es númerico
        y si está activo.

        Parámetros:
        @IsActive:
            - NULL : Retorna todos los registros
            - 1    : Retorna solo documentos activos
            - 0    : Retorna solo documentos inactivos
    */

    SELECT
        Id,
        Name,
        Abbreviation,
        Country,
        Length,
        IsNumeric,
        IsActive
    FROM IdentityDocument
    WHERE (@IsActive IS NULL OR IsActive = @IsActive)
    ORDER BY Name;
END;
GO