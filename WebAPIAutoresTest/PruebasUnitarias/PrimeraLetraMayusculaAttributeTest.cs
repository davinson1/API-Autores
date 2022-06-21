using System.ComponentModel.DataAnnotations;
using WebAPIAutores.Entidades.Validaciones;

namespace WebAPIAutoresTest.PruebasUnitarias
{
    [TestClass]
    public class PrimeraLetraMayusculaAttributeTest
    {
        [TestMethod]
        public void PrimeraLetraMinuscula_DevuelveError()
        {
            //preparacion
            var primeraLetraMayuscula = new PrimeraLetraMayusculaAttribute();
            var valor = "felipe";
            var valCOntext = new ValidationContext(new { Nombre = valor });

            //ejecucacion
            var resultado = primeraLetraMayuscula.GetValidationResult(valor, valCOntext);

            //verificacion
            Assert.AreEqual("La primera letra del campo Nombre debe ser mayuscula.", resultado.ErrorMessage);
        }


        [TestMethod]
        public void ValorNulo_NoDevuelveError()
        {
            //preparacion
            var primeraLetraMayuscula = new PrimeraLetraMayusculaAttribute();
            string valor = null;
            var valCOntext = new ValidationContext(new { Nombre = valor });

            //ejecucacion
            var resultado = primeraLetraMayuscula.GetValidationResult(valor, valCOntext);

            //verificacion
            Assert.IsNull(resultado);
        }

        [TestMethod]
        public void ValorCOnPrimeraLetraMayuscula_NoDevuelveError()
        {
            //preparacion
            var primeraLetraMayuscula = new PrimeraLetraMayusculaAttribute();
            string valor = "Felipe";
            var valCOntext = new ValidationContext(new { Nombre = valor });

            //ejecucacion
            var resultado = primeraLetraMayuscula.GetValidationResult(valor, valCOntext);

            //verificacion
            Assert.IsNull(resultado);
        }
    }
}