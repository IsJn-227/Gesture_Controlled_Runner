import cv2
import mediapipe as mp
import time
import pyautogui

# --- Parameters ---
FAST_SWIPE_THRESHOLD = 80       # Lower = more sensitive to fast swipes
COOLDOWN_FAST = 0.08            # Lower cooldown = faster consecutive swipes
JUMP_COOLDOWN = 0.25

# MediaPipe setup
mp_hands = mp.solutions.hands
hands = mp_hands.Hands(max_num_hands=1, min_detection_confidence=0.7)
mp_draw = mp.solutions.drawing_utils

cap = cv2.VideoCapture(0)

prev_x = None
prev_time = None
last_swipe_time = 0
last_jump_time = 0

def is_fingers_curled(landmarks):
    curled = 0
    finger_tip_ids = [8, 12, 16, 20]
    finger_base_ids = [6, 10, 14, 18]
    wrist_y = landmarks[0].y

    for tip_id, base_id in zip(finger_tip_ids, finger_base_ids):
        if landmarks[tip_id].y > landmarks[base_id].y:
            curled += 1

    index_tip = landmarks[8]
    wrist = landmarks[0]
    dist_to_wrist = abs(index_tip.y - wrist.y)
    return curled >= 3 and dist_to_wrist < 0.2

while True:
    success, frame = cap.read()
    if not success:
        break

    frame = cv2.flip(frame, 1)
    rgb = cv2.cvtColor(frame, cv2.COLOR_BGR2RGB)
    results = hands.process(rgb)

    current_time = time.time()

    if results.multi_hand_landmarks:
        for hand_landmarks in results.multi_hand_landmarks:
            mp_draw.draw_landmarks(frame, hand_landmarks, mp_hands.HAND_CONNECTIONS)

            x = hand_landmarks.landmark[0].x  # Use wrist for more stable motion tracking

            if prev_x is not None and prev_time is not None:
                dx = (x - prev_x) * 1000
                dt = current_time - prev_time
                speed = dx / dt if dt > 0 else 0

                # Debug: show speed
                cv2.putText(frame, f"Speed: {int(speed)}", (10, 50),
                            cv2.FONT_HERSHEY_SIMPLEX, 1, (255, 255, 0), 2)

                if current_time - last_swipe_time > COOLDOWN_FAST:
                    if speed > FAST_SWIPE_THRESHOLD:
                        pyautogui.press("right")
                        print("MOVE_RIGHT")
                        last_swipe_time = current_time
                    elif speed < -FAST_SWIPE_THRESHOLD:
                        pyautogui.press("left")
                        print("MOVE_LEFT")
                        last_swipe_time = current_time

            prev_x = x
            prev_time = current_time

            if current_time - last_jump_time > JUMP_COOLDOWN:
                if is_fingers_curled(hand_landmarks.landmark):
                    pyautogui.press("up")
                    print("JUMP")
                    last_jump_time = current_time

    cv2.imshow("Gesture Control", frame)
    if cv2.waitKey(1) & 0xFF == ord("q"):
        break

cap.release()
cv2.destroyAllWindows()
