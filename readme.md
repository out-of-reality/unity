# 🎮 Proyecto Unity: project_final_game

## 📝 Descripción General

### Una aplicación de realidad virtual y aumentada basada en Unity, diseñada para fisioterapia y rehabilitación. Este cliente de juego se conecta a la plataforma Out of Reality para ofrecer ejercicios de rehabilitación inmersivos y atractivos.

## 🎯 Resumen

Project Final Game es la aplicación frontend de Unity para el sistema de rehabilitación Out of Reality. Ofrece un entorno interactivo y gamificado donde los pacientes pueden realizar ejercicios de fisioterapia en realidad virtual, lo que hace que la rehabilitación sea más atractiva y efectiva.


## 📁 Estructura del Proyecto

La organización de carpetas es la siguiente:

```
project_final_game/
│
├── Assets/
│   ├── Scenes/         # Escenas del juego (.unity)
│   ├── Scripts/        # Scripts C# de la lógica del juego
│   ├── Prefabs/        # Prefabs reutilizables
│   ├── Materials/      # Materiales y texturas
│   ├── Audio/          # Archivos de audio y música
│   └── ...             # Otros recursos (animaciones, UI, etc.)
│
├── ProjectSettings/    # Configuración del proyecto Unity
├── Packages/           # Dependencias y paquetes de Unity
├── Library/            # Archivos generados automáticamente (ignorar)
├── Logs/               # Logs de ejecución y depuración
├── Temp/               # Archivos temporales (ignorar)
├── UserSettings/       # Configuración de usuario local
└── .vscode/            # Configuración para Visual Studio Code
```

---

## 🚀 Instalación y Ejecución


### Clonar el repositorio:
```
git clone https://github.com/out-of-reality/project_final_game.git
cd project_final_game
```

### Abrir Unity Hub:
- Haz clic en "Abrir" y selecciona la carpeta project_final_game
- Unity importará las dependencias automáticamente

### Configurar la conexión de la API:

- Ve a Assets/Scripts/Poses/UdpClientScript.cs
- Actualiza los endpoints de la API para que coincidan con tu backend de Out of Reality
- Configura los ajustes de autenticación

Ejecutar la aplicación:

- Abre la escena principal desde Assets/Scenes/MainMenu.unity
- Pulsa "Reproducir" en el editor de Unity o compila para tu plataforma de destino

## 🗂️ Acceso a Carpetas Principales

### 1. Escenas (`Assets/Scenes/`)
- Contiene todos los archivos de escena (`.unity`).
- Para abrir una escena:
  1. Abre Unity y ve al panel **Project**.
  2. Navega a `Assets/Scenes/`.
  3. Haz doble clic en la escena que deseas abrir.

### 2. Scripts (`Assets/Scripts/`)
- Aquí están todos los scripts C# del juego.
- Para editarlos:
  1. Navega a `Assets/Scripts/` en el panel **Project**.
  2. Haz doble clic en el script a modificar.
  3. El script se abrirá en tu editor de código preferido.

---

## 🖥️ Requisitos

- **Unity:** 22.3.42f1.
- **Editor de código:** Visual Studio Code.
- **Git**

---

## ⚠️ Notas Importantes

- Las carpetas `Library/`, `Temp/`, `Logs/` y `UserSettings/` incluidas en el `.gitignore` deben ser descargadas dentro del editor de unity.
