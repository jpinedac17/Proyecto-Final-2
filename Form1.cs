using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using Microsoft.Data.SqlClient;
using ProyectoFinal1;



namespace ProyectoFinal1
{

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private async void btnBusqueda_Click(object sender, EventArgs e)
        {
            string prompt = txtPrompt.Text;

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer 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");

                var body = new
                {
                    model = "mistralai/mistral-7b-instruct:free",
                    messages = new[]
                    {
                        new { role = "user", content = $"Eres un experto en finanzas personales con años de experiencia ayudando a personas a crear presupuestos, reducir gastos innecesarios y tomar decisiones financieras inteligentes. Tu tarea es actuar como un asesor financiero personalizado. \r\n\r\nCuando recibas los datos del usuario (ingresos, gastos, deudas, metas financieras, etc.), analiza su situación y proporciona:\r\n\r\n1. Un presupuesto mensual personalizado.\r\n2. Consejos para ahorrar dinero y reducir gastos innecesarios.\r\n3. Recomendaciones para mejorar su salud financiera a corto y largo plazo.\r\n4. Ideas para invertir o planificar su futuro financiero (según el perfil del usuario).\r\n\r\nSé claro, práctico y empático. Adapta tus respuestas al contexto financiero que el usuario proporciona. No uses lenguaje técnico innecesario.\r\n" + prompt}
                    }
                };

                string json = JsonConvert.SerializeObject(body);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync("https://openrouter.ai/api/v1/chat/completions", content);
                string result = await response.Content.ReadAsStringAsync();

                dynamic respuesta = JsonConvert.DeserializeObject(result);
                txtResultado.Text = respuesta.choices[0].message.content;

                // Guardar en la base de datos
                Funciones.GuardarEnBD(prompt, txtResultado.Text);

                // Crear carpeta y crear archivos
                string rutaEscritorio = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string rutaCarpeta = Path.Combine(rutaEscritorio, "PresupuestoAI");

                if (!Directory.Exists(rutaCarpeta))
                {
                    Directory.CreateDirectory(rutaCarpeta);
                }

                string rutaWord = Path.Combine(rutaCarpeta, "Resultado.docx");

                Funciones.CrearDocumentoWord(txtResultado.Text, txtPrompt.Text, rutaWord);

                // Reiniciar texto del usuario
                txtPrompt.Text = "";
            }
        }
    }
}
