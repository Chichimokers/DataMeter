# 📊 Data Counter

¡Bienvenido a **Data Counter**! 🚀  
Este proyecto es una aplicación de Windows Forms que te permite monitorear el tráfico de red (subida y bajada) en tiempo real para cualquier adaptador de red en tu dispositivo. 🖥️

---

## 📝 Características

- 🎛️ **Selección de Adaptadores**: Muestra todos los adaptadores de red disponibles en tu dispositivo para monitorear.  
- 📡 **Monitoreo en Tiempo Real**: Calcula el tráfico total (subida + bajada) desde que inicias el programa.  
- 📈 **Interfaz Intuitiva**: Actualiza automáticamente el consumo de datos cada segundo.  

---

## 🛠️ Tecnologías Utilizadas

- 💻 **Lenguaje**: C#  
- 🖼️ **Framework**: Windows Forms  
- 🌐 **Clases de Red**: `System.Net.NetworkInformation`  

---

## 🚀 Cómo Usar

1. 🔧 **Configura el Proyecto**:
   - Asegúrate de tener Visual Studio instalado en tu equipo.
   - Clona o descarga este repositorio.  

2. ▶️ **Ejecuta la Aplicación**:
   - Compila y ejecuta el proyecto desde Visual Studio.
   - La aplicación detectará automáticamente todos los adaptadores de red.

3. 📋 **Selecciona un Adaptador**:
   - Usa el menú desplegable (ComboBox) para seleccionar el adaptador de red que deseas monitorear.

4. 📡 **Monitorea el Tráfico**:
   - Observa en tiempo real cómo el tráfico (subida + bajada) se muestra en el `label1`, actualizado en megabytes (MB).

---

## 📷 Captura de Pantalla

_(Aquí podrías añadir una captura de pantalla de la aplicación mostrando la funcionalidad)_

---

## 🛡️ Requisitos del Sistema

- 🖥️ Windows 7 o superior  
- 🔧 .NET Framework 4.7.2 o superior  
- 🛠️ Visual Studio con soporte para Windows Forms  

---

## 📚 Cómo Funciona el Proyecto

1. **Detección de Adaptadores**:  
   Utiliza la clase `NetworkInterface` para listar todos los adaptadores de red disponibles.  

2. **Monitoreo en Tiempo Real**:  
   Con un `Timer`, el programa calcula el tráfico enviado y recibido desde que comienza el monitoreo.  

3. **Conversión a MB**:  
   Los datos en bytes se convierten a megabytes para que sean más fáciles de entender.  

---

## 📂 Estructura del Proyecto

