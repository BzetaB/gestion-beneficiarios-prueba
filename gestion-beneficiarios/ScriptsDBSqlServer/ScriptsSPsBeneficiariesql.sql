USE GestionBeneficiariosDB;
GO

CREATE PROCEDURE sp_Beneficiary_GetDocumentNumbers
    @IsActive BIT = NULL,
    @Country VARCHAR(50) = NULL
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
        d.Country,
        d.IsActive
    FROM Beneficiary b
    INNER JOIN IdentityDocument d
        ON b.IdentityDocumentId = d.Id
    WHERE (@IsActive IS NULL OR d.IsActive = @IsActive)
    AND (@Country IS NULL OR d.Country = @Country);
END;
GO



CREATE PROCEDURE sp_Beneficiaries_Search
    @SearchTerm NVARCHAR(100),
    @IsActive BIT = NULL,
    @Country NVARCHAR(100) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    /*
        Descripción: 
        Busca beneficiarios por nombre, apellido o número de documento de identidad.
        Permite filtrar opcionalmente por estado del documento (activo/inactivo) y país.

        Parámetros:
            @SearchTerm - Término a buscar en nombre, apellido o número de documento
            @IsActive   - Estado del documento (NULL = todos, 1 = activos, 0 = inactivos)
            @Country    - País del documento (NULL o 'all' = todos los países)
    */

    SELECT 
        b.FirstName,
        b.LastName,
        b.DocumentNumber,
        id.Abbreviation AS IdentityDocument,
        id.Country,
        id.IsActive
    FROM 
        dbo.Beneficiary b
    INNER JOIN 
        dbo.IdentityDocument id ON b.IdentityDocumentId = id.Id
    WHERE 
        (
            -- Búsqueda por nombre
            b.FirstName LIKE '%' + @SearchTerm + '%'
            OR 
            -- Búsqueda por apellido
            b.LastName LIKE '%' + @SearchTerm + '%'
            OR 
            -- Búsqueda por nombre completo
            CONCAT(b.FirstName, ' ', b.LastName) LIKE '%' + @SearchTerm + '%'
            OR 
            -- Búsqueda por documento
            b.DocumentNumber LIKE '%' + @SearchTerm + '%'
        )
        AND 
        -- Filtro de estado (si se proporciona)
        (@IsActive IS NULL OR id.IsActive = @IsActive)
        AND 
        -- Filtro de país (si se proporciona)
        (@Country IS NULL OR id.Country = @Country)
    ORDER BY 
        b.FirstName, b.LastName;
END;
GO