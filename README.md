# 🎵 RanadaProject - Tematica Fondas 18 🍳

<div align="center">

![Unity](https://img.shields.io/badge/Unity-2D-FF9500?style=for-the-badge&logo=unity&logoColor=white)
![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)
![MIDI](https://img.shields.io/badge/MIDI-Support-purple?style=for-the-badge&logo=music&logoColor=white)
![Status](https://img.shields.io/badge/Status-Game_Jump_Project-green?style=for-the-badge)

_Un juego de ritmo que combina las ramadas con sus deliciosas comidas tipicas_ 🎶

</div>

## 📖 Descripción

**RanadaProject** es un innovador juego de ritmo desarrollado en Unity donde los jugadores deben presionar teclas al compás de la música para ayudar a la rana a comer todo lo que pueda. Con una adorable rana chilena como protagonista, el juego combina mecánicas de rhythm game con una temática culinaria única y colorida basada en Chile.

### ✨ Características Principales

- 🎹 **Sistema de Notas MIDI**: Utiliza archivos MIDI reales para generar las secuencias de juego
- 🍎 **Ingredientes Variados**: Colecciona diferentes tipos de comida que caen al ritmo
- 🐸 **Protagonista Carismático**: Juega como una rana animada con múltiples estados
- 🎯 **Sistema de Puntuación Avanzado**: Rankings de S a F basados en precisión
- 🏆 **Sistema de Combos**: Multiplica tu puntuación con combos perfectos
- 🎨 **Efectos Visuales**: Animaciones de pulso sincronizadas con el beat
- 🔊 **Audio Dinámico**: Gestión completa de música y efectos de sonido
- ⏸️ **Menú de Pausa**: Sistema completo con countdown para reanudar

## 🎮 Cómo Jugar

1. **Presiona las teclas** `↑` y `↓` (o usa un gamepad) al ritmo de la música
2. **Captura los ingredientes** que caen sincronizados con las notas
3. **Mantén el combo** para multiplicar tu puntuación
4. **Consigue el mejor ranking** posible (S, A, B, C, D, E, F)

### 🎯 Sistema de Precisión

- 🟢 **Perfect**: ±0.5 segundos - Máxima puntuación
- 🟡 **Great**: ±1.0 segundos - Buena puntuación
- 🟠 **Good**: >±1.0 segundos - Puntuación básica
- 🔴 **Miss**: Sin input o fuera de tiempo

## 🛠️ Tecnologías Utilizadas

- **Engine**: Unity 2D
- **Lenguaje**: C#
- **Audio**: Melanchall.DryWetMidi para procesamiento MIDI
- **UI**: TextMeshPro con efectos de color arcoíris
- **Input System**: Unity Input System con soporte para teclado y gamepad
- **Animaciones**: Animator Controller para personajes y efectos

## 📁 Estructura del Proyecto

```
Assets/
├── Script/
│   ├── Animation/        # Efectos visuales y pulsos
│   ├── Food/            # Sistema de ingredientes
│   ├── Interface/       # UI y gestión de sonido
│   ├── Notes/           # Lógica de notas y lanes
│   ├── Player/          # Input y puntuación
│   └── Songs/           # Gestión de MIDI y BPM
├── Audio/               # Música y efectos de sonido
├── Scenes/              # Escenas del juego
├── PreFab/              # Prefabs reutilizables
├── Texture/             # Sprites y texturas
└── StreamingAssets/     # Archivos MIDI
```

## 🎵 Sistema de Audio

El juego cuenta con un **SoundManager** centralizado que maneja:

- 🎼 Música de fondo (BGM) adaptable por escena
- 🔊 Efectos de sonido (SFX) con volumen independiente
- 🎤 Audio del menú de pausa
- 📊 Sliders de volumen con persistencia de configuración

## 🏆 Sistema de Puntuación

- **Score Base**: 10 puntos por nota
- **Combos**: Multiplicador a partir de 5+ hits consecutivos
- **Rankings**: S (70k+), A (60k+), B (50k+), C (40k+), D (30k+), E (20k+), F (<20k)
- **Estadísticas**: Tracking completo de Perfect, Great, Good y Miss

## 🎨 Efectos Visuales

- ✨ **Pulsos Sincronizados**: Los elementos de UI pulsan al ritmo de la música
- 🌈 **Texto Arcoíris**: Efectos de color dinámicos en el texto
- 🎯 **Feedback Visual**: Animaciones para cada tipo de hit
- 🐸 **Animaciones de Personaje**: Estados de éxito, fallo y miss

## 🎮 Controles

| Acción | Teclado           | Gamepad         |
| ------ | ----------------- | --------------- |
| Lane 1 | ↑ (Flecha Arriba) | Y (Botón Norte) |
| Lane 2 | ↓ (Flecha Abajo)  | A (Botón Sur)   |
| Pausa  | Esc               | Start           |
| Click  | Click Mouse       | A               |

## 🚀 Instalación y Ejecución

1. **Clona el repositorio**

   ```bash
   git clone https://github.com/tu-usuario/RanadaProject.git
   ```

2. **Abre en Unity**

   - Unity 6.0 LTS o superior
   - TextMeshPro viene incluido por defecto
   - Input System viene habilitado automáticamente

3. **Dependencias**

   - Melanchall.DryWetMidi.dll (incluido en Assets)
   - Unity Input System

4. **Ejecuta**
   - Abre la escena principal y presiona Play

## 👥 Créditos del Equipo

### 🎮 **Liderazgo del Proyecto**

- **🎮 Líder de Equipo**: Luis Lagos
- **🎨 Líder de Ilustración**: Astrid Kobrock
- **💻 Líder de Programación**: Emilio Herrera
- **🎵 Líder de Sonido**: Pablo Pessenti

### 💻 **Equipo de Programación**

- **Emilio Herrera** - _Lead Programmer_
- **Esteban Oñate** - _Programmer_
- **Enrique Marín** - _Programmer_
- **Fabián Quiñónez** - _Programmer_

### 🎨 **Equipo de Ilustración**

- **Astrid Kobrock** - _Lead Artist_
- **Nella Barrera** - _Artist_
- **Aurora Infante** - _Artist_
- **Alexandra Guzmán** - _Artist_
- **Maite Treulén** - _Artist_

### 🎵 **Equipo de Sonido**

- **Pablo Pessenti** - _Lead Sound Designer_
- **Fabián Quiñónez** - _Sound Designer_
- **Alan Alarcón** [@alanmurphycl](https://github.com/alanmurphycl) - _Ingeniero de Grabación_

### 💡 **Concepto Original**

- **Sofía Morales** [@justberrie](https://github.com/justberrie) - _Idea Original_

---

### 🛠️ **Tecnologías Implementadas**

- **Unity Engine 2D** - Framework principal de desarrollo
- **C# Scripting** - Lógica de juego y sistemas
- **Melanchall.DryWetMidi** - Procesamiento de archivos MIDI
- **Unity Input System** - Manejo moderno de controles
- **TextMeshPro** - Sistema de texto con efectos visuales

---

<div align="center">

**🎵 ¡Disfruta cocinando al ritmo de la música! 🍳**

_Proyecto Game Jump - Desarrollado con ❤️ y mucha música_

</div>