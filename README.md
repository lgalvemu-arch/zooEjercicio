Queremos varias cosas		
Back
Trabajadores del Zoo.
	Rol: Introductor de datos.
		Tiene acceso a la introducción modificación y lectura de todas las tablas.
Rol: Alimentador
	Tiene que pedir comida al proveedor cuando el stock baje del stock mínimo, para saber cuanto tiene que pedir, se sumará todo lo que hay en dosis. Ejemplo. Si tenemos como alimento calamares tenemos un stock de 10 y un stock mínimo de 15 tenemos que pedir porque estamos por debajo del stock mínimo, cuanto pedimos?, vamos a dosis y calculamos en todas las dosis cuanto necesitaremos, si la cantidad es superior a 5, lo pedimos superior a 5 y si es inferior pedimos 5.
	
Rol: Veterinario:
	Tiene que calcular cuales son las calorías que ingiere un animal.
	Tiene que poner animales compatibles en las jaulas.
		carnívoro no con herbívoro.
		Omnívoro no con herbívoro.
Gerente del Zoo.
	Quiere saber  cuanto se gasta por animal.
	Quiera saber cuánto dinero tiene invertido en comida.
Front
	Visitantes del Zoo.
	Quiero que puedan ver una web para mostar cada animal que tenemos en el Zoo con su ubicación y ecosistema.
	Vamos a crear un ViewComponent que se va ha llamar ViewComponetAnimal. Contendrá los datos de animales. Para cada animal. El nombre, el ecosistema, la Dieta, la foto y la jaula donde está.
	Ejemplo, un visitante quiere selecciona dentro de especies, serpientes, y le aparecerá un carrusel con esta información de todos eso animales.
	En el home aparecerán todos los animales con esa información, sin filtrar.
