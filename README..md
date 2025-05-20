# 📱 ARchitect - AR Building Recognition App

An Augmented Reality mobile app that detects university buildings using YOLOv8 and displays real-time information using AR overlays.

 🔍 How it works:
1. User scans the building with their mobile camera.
2. A screenshot is sent to a Flask backend.
3. YOLOv8 processes the image and identifies the building.
4. The app overlays building info in AR using Unity.

 🧠 Technologies:
- Unity 3D
- YOLOv8 (via Roboflow)
- Flask API
- Google Colab (for training)
- Computer Vision
- Image Processing

 📸 Screenshots
![screenshot1](Screenshots/screen1.png)
...

 🚀 Getting Started
1. Clone the repo
2. Install Flask backend dependencies:
```bash
pip install -r FlaskServer/requirements.txt
