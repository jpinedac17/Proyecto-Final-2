using System;
using Microsoft.Data.SqlClient;

using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

using Word = DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.Identity.Client;
namespace ProyectoFinal1
{
    public static class Funciones
    {
        public static void GuardarEnBD(string prompt, string respuesta)
        {
            string connectionString = "Data Source=DESKTOP-DVB2F0D\\SQLEXPRESS;Initial Catalog=InvestigacionesAI;Integrated Security=True;TrustServerCertificate=true";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO Investigaciones (Prompt, Respuesta) VALUES (@Prompt, @Respuesta)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Prompt", prompt);
                    command.Parameters.AddWithValue("@Respuesta", respuesta);
                    command.ExecuteNonQuery();
                }
            }
        }

        public static void CrearDocumentoWord(string respuesta, string prompt, string rutaArchivo)
        {
            using (WordprocessingDocument wordDocument =
        WordprocessingDocument.Create(rutaArchivo, WordprocessingDocumentType.Document))
            {
                MainDocumentPart mainPart = wordDocument.AddMainDocumentPart();
                mainPart.Document = new Document();
                Body body = mainPart.Document.AppendChild(new Body());

                // Crear el título del documento (Prompt)
                Paragraph titulo = new Paragraph();
                Run runTitulo = new Run();
                runTitulo.Append(new Text(prompt));

                // Aplicar estilo de título
                RunProperties propsTitulo = new RunProperties();
                propsTitulo.Append(new RunFonts() { Ascii = "Calibri" }); // Fuente
                propsTitulo.Append(new FontSize() { Val = "28" }); // 14 pt (14 * 2)
                propsTitulo.Append(new Bold());
                runTitulo.PrependChild(propsTitulo);
                titulo.Append(runTitulo);
                body.Append(titulo);

                // Espaciado
                body.Append(new Paragraph(new Run(new Break())));

                // Crear el cuerpo del documento (Respuesta)
                Paragraph cuerpo = new Paragraph();
                Run runCuerpo = new Run();
                runCuerpo.Append(new Text(respuesta));

                // Aplicar estilo normal
                RunProperties propsCuerpo = new RunProperties();
                propsCuerpo.Append(new RunFonts() { Ascii = "Calibri" });
                propsCuerpo.Append(new FontSize() { Val = "24" }); // 12 pt
                runCuerpo.PrependChild(propsCuerpo);
                cuerpo.Append(runCuerpo);
                body.Append(cuerpo);

                mainPart.Document.Save();
            }
        }
    }
}
