# 🏛️ ARchitect — AR App for Building Recognition

An Augmented Reality mobile application that identifies university buildings using YOLOv8 and displays real-time information using AR overlays.

🎓 This was my Bachelor's Capstone Project in IT Engineering, developed over two months.

---

## 🔍 How It Works

1. User opens the app and navigates to the AR screen.
2. The mobile camera scans a building.
3. A screenshot is sent to a Flask backend.
4. YOLOv8 processes the image and identifies the building.
5. Unity overlays the building’s information in AR on the live camera feed.

---

## 🧠 Technologies Used

- Unity 3D (for AR interface and UI)
- YOLOv8 (trained via Roboflow)
- Python Flask API (backend server)
- Google Colab (for training and fine-tuning the model)
- Computer Vision
- Image Processing

---

## 🚀 Getting Started

### 1. Clone the Repository
```bash
git clone https://github.com/Bita-Batmani/ARchitect-FinalUniProject
cd ARchitect
