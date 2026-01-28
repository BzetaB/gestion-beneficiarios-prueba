USE GestionBeneficiariosDB;
GO

CREATE PROCEDURE sp_Beneficiary_GetDocumentNumbers
    @IsActive BIT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    /*
        Descripción:
        Retorna el listado de beneficiarios con sus números de documento,
        incluyendo nombres, apellidos, tipo de documento (abreviatura)
        y el estado del documento.

        Parámetros:
        @IsActive:
            - NULL : Retorna todos los registros
            - 1    : Retorna solo documentos activos
            - 0    : Retorna solo documentos inactivos
    */

    SELECT
        b.FirstName,
        b.LastName,
        b.DocumentNumber,
        d.Abbreviation AS IdentityDocument,
        d.IsActive
    FROM Beneficiary b
    INNER JOIN IdentityDocument d
        ON b.IdentityDocumentId = d.Id
    WHERE (@IsActive IS NULL OR d.IsActive = @IsActive);
END;
GO