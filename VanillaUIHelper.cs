using SDG.Unturned;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace SeniorS.UIHelper
{
    public class VanillaUIHelper : EditorWindow
    {
        [MenuItem("Tools/Vanilla UI Helper")]
        public static void OpenWindow()
        {
            GetWindow<VanillaUIHelper>("Vanilla UI Helper");
        }

        private void OnGUI()
        {
            GUILayout.Label("Vanilla UI Helper", EditorStyles.boldLabel);
            GUILayout.Label("by SeniorS", EditorStyles.miniLabel);
            EditorGUILayout.Space(8);

            GUILayout.Label("Essentials", EditorStyles.boldLabel);
            EditorGUILayout.Space(2);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Add button", EditorStyles.miniButtonLeft))
            {
                AddButton();
            }
            if (GUILayout.Button("Add toggle", EditorStyles.miniButtonRight)) 
            {
                AddToggle();
            }
            GUILayout.EndHorizontal();
            EditorGUILayout.Space(2);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Add InputField", EditorStyles.miniButtonLeft))
            {
                AddInputField();
            }
            if (GUILayout.Button("Add ScrollView", EditorStyles.miniButtonRight))
            {
                AddScrollView();
            }
            GUILayout.EndHorizontal();
            EditorGUILayout.Space(2);

            if (GUILayout.Button("Setup Canvas"))
            {
                if (!CheckSelection()) return;
                GameObject effectGameObject = Selection.activeGameObject;
                if(!effectGameObject.name.Equals("Effect"))
                {
                    EditorUtility.DisplayDialog("Vanilla UI Helper", "Canvas must only be added to gameobject called 'Effect'.", "OK");
                    return;
                }
                GameObject canvasGameObject = new GameObject("Canvas", typeof(RectTransform), typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
                canvasGameObject.transform.SetParent(effectGameObject.transform);
                RectTransform rectTransform = canvasGameObject.GetComponent<RectTransform>();
                SetupRectTransform(rectTransform);
                
                Canvas canvas = canvasGameObject.GetComponent<Canvas>();
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                canvas.sortingOrder = 0;
                canvas.pixelPerfect = true;
                canvas.targetDisplay = 0;
                canvas.additionalShaderChannels = (AdditionalCanvasShaderChannels)011001; // TextCoord1, Normal, Tangent

                CanvasScaler scaler = canvasGameObject.GetComponent<CanvasScaler>();
                scaler.uiScaleMode = CanvasScaler.ScaleMode.ConstantPixelSize;
                scaler.scaleFactor = 1f;
                scaler.referencePixelsPerUnit = 25;

                UnturnedCanvasScaler unturnedCanvasScaler = canvasGameObject.AddComponent<UnturnedCanvasScaler>();
                unturnedCanvasScaler.scaler = scaler;
            }

            EditorGUILayout.Space(50);
            if (GUILayout.Button("Get Unturned UI Assets"))
            {
                Application.OpenURL("https://github.com/DanielWillett/UnturnedUIAssets/tree/main");
            }
        }

        private void AddButton()
        {
            if (!CheckSelection()) return;

            GameObject buttonGameObject = new GameObject("Button", typeof(RectTransform), typeof(Image), typeof(Button));
            buttonGameObject.layer = 5;
            buttonGameObject.transform.SetParent(Selection.activeTransform);
            RectTransform rectTransform = buttonGameObject.GetComponent<RectTransform>();
            SetupRectTransform(rectTransform, 160, 30);
            Image image = buttonGameObject.GetComponent<Image>();
            image.color = new Color(0.9f, 0.9f, 0.9f);
            image.type = Image.Type.Sliced;
            image.pixelsPerUnitMultiplier = 0.6f;
            image.sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Editor/VanillaUIHelper/DarkTheme/Box.png");
            Button button = buttonGameObject.GetComponent<Button>();
            button.transition = Selectable.Transition.SpriteSwap;
            button.spriteState = new SpriteState
            {
                highlightedSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Editor/VanillaUIHelper/DarkTheme/Box_Highlighted.png"),
                pressedSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Editor/VanillaUIHelper/DarkTheme/Box_Pressed.png")
            };

            GameObject textGameObject = new GameObject("Text", typeof(RectTransform), typeof(TextMeshProUGUI));
            textGameObject.layer = 5;
            textGameObject.transform.SetParent(buttonGameObject.transform);
            RectTransform textRectTransform = textGameObject.GetComponent<RectTransform>();
            SetupRectTransform(textRectTransform, 160, 30);
            StrecthRectTransform(textRectTransform);
            TextMeshProUGUI text = textGameObject.GetComponent<TextMeshProUGUI>();
            text.text = "Button";
            text.fontSize = 20;
            text.color = Color.white;
            text.alignment = TextAlignmentOptions.Center;
        }

        private void AddToggle()
        {
            if (!CheckSelection()) return;

            GameObject buttonGameObject = new GameObject("Toggle", typeof(RectTransform), typeof(Image), typeof(Button));
            buttonGameObject.layer = 5;
            buttonGameObject.transform.SetParent(Selection.activeTransform);
            RectTransform rectTransform = buttonGameObject.GetComponent<RectTransform>();
            SetupRectTransform(rectTransform, 30, 30);
            Image image = buttonGameObject.GetComponent<Image>();
            image.color = new Color(0.9f, 0.9f, 0.9f);
            image.type = Image.Type.Sliced;
            image.pixelsPerUnitMultiplier = 0.15f;
            image.sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Editor/VanillaUIHelper/DarkTheme/Box.png");
            Button button = buttonGameObject.GetComponent<Button>();
            button.transition = Selectable.Transition.SpriteSwap;
            button.spriteState = new SpriteState
            {
                highlightedSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Editor/VanillaUIHelper/DarkTheme/Box_Highlighted.png"),
                pressedSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Editor/VanillaUIHelper/DarkTheme/Box_Pressed.png")
            };

            GameObject checkGameobject = new GameObject("CheckIcon", typeof(RectTransform), typeof(Image));
            checkGameobject.layer = 5;
            checkGameobject.transform.SetParent(buttonGameObject.transform);
            RectTransform checkRectTransform = checkGameobject.GetComponent<RectTransform>();
            SetupRectTransform(checkRectTransform, 50, 50);
            checkRectTransform.anchoredPosition = new Vector2(0, -1);
            image = checkGameobject.GetComponent<Image>();
            image.sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Editor/VanillaUIHelper/DarkTheme/Toggle_Foreground.png");
            image.color = Color.white;
            checkGameobject.SetActive(false);
        }

        private void AddInputField()
        {
            if (!CheckSelection()) return;

            GameObject inputFieldGameObject = new GameObject("InputField", typeof(RectTransform), typeof(Image), typeof(TMP_InputField));
            inputFieldGameObject.layer = 5;
            inputFieldGameObject.transform.SetParent(Selection.activeTransform);
            RectTransform rectTransform = inputFieldGameObject.GetComponent<RectTransform>();
            SetupRectTransform(rectTransform, 140, 30);
            Image image = inputFieldGameObject.GetComponent<Image>();
            image.color = new Color(0.9f, 0.9f, 0.9f);
            image.type = Image.Type.Sliced;
            image.pixelsPerUnitMultiplier = 0.6f;
            image.sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Editor/VanillaUIHelper/DarkTheme/Box.png");
            TMP_InputField inputField = inputFieldGameObject.GetComponent<TMP_InputField>();
            inputField.transition = Selectable.Transition.SpriteSwap;
            inputField.spriteState = new SpriteState
            {
                highlightedSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Editor/VanillaUIHelper/DarkTheme/Box_Highlighted.png"),
                pressedSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Editor/VanillaUIHelper/DarkTheme/Box_Pressed.png")
            };

            GameObject TextAreaGameObject = new GameObject("Text Area", typeof(RectTransform), typeof(RectMask2D));
            TextAreaGameObject.layer = 5;
            TextAreaGameObject.transform.SetParent(inputFieldGameObject.transform);
            RectTransform textAreaRectTransform = TextAreaGameObject.GetComponent<RectTransform>();
            SetupRectTransform(textAreaRectTransform, 140, 30);
            StrecthRectTransform(textAreaRectTransform);

            inputField.textViewport = textAreaRectTransform;

            GameObject textGameObject = new GameObject("Text", typeof(RectTransform), typeof(TextMeshProUGUI));
            textGameObject.transform.SetParent(TextAreaGameObject.transform);
            RectTransform textRectTransform = textGameObject.GetComponent<RectTransform>();
            SetupRectTransform(textRectTransform, 140, 30);
            StrecthRectTransform(textRectTransform);
            TextMeshProUGUI text = textGameObject.GetComponent<TextMeshProUGUI>();
            text.fontSize = 14;
            text.color = Color.white;
            text.alignment = TextAlignmentOptions.Center;

            GameObject placeholderGameObject = new GameObject("Placeholder", typeof(RectTransform), typeof(TextMeshProUGUI));
            placeholderGameObject.layer = 5;
            placeholderGameObject.transform.SetParent(TextAreaGameObject.transform);
            RectTransform placeholderRectTransform = placeholderGameObject.GetComponent<RectTransform>();
            SetupRectTransform(placeholderRectTransform, 140, 30);
            StrecthRectTransform(placeholderRectTransform);
            TextMeshProUGUI placeholder = placeholderGameObject.GetComponent<TextMeshProUGUI>();
            placeholder.text = "Enter text...";
            placeholder.fontSize = 14;
            placeholder.color = new Color(0.7f, 0.7f, 0.7f);
            placeholder.alignment = TextAlignmentOptions.Center;

            inputField.textComponent = text;
            inputField.placeholder = placeholder;
        }

        private void AddScrollView()
        {
            if (!CheckSelection()) return;

            GameObject scrollViewGameObject = new GameObject("ScrollView", typeof(RectTransform), typeof(Image), typeof(ScrollRect));
            scrollViewGameObject.layer = 5;
            scrollViewGameObject.transform.SetParent(Selection.activeTransform);
            RectTransform rectTransform = scrollViewGameObject.GetComponent<RectTransform>();
            SetupRectTransform(rectTransform, 200, 200);
            Image image = scrollViewGameObject.GetComponent<Image>();
            image.color = new Color(0f, 0f, 0f, 0f);
            image.sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Editor/VanillaUIHelper/DarkTheme/Box.png");
            ScrollRect scrollRect = scrollViewGameObject.GetComponent<ScrollRect>();
            scrollRect.movementType = ScrollRect.MovementType.Clamped;
            scrollRect.scrollSensitivity = 5;
            scrollRect.verticalScrollbarVisibility = ScrollRect.ScrollbarVisibility.Permanent;
            scrollRect.horizontal = false;

            GameObject viewportGameObject = new GameObject("Viewport", typeof(RectTransform), typeof(Image), typeof(Mask));
            viewportGameObject.layer = 5;
            viewportGameObject.transform.SetParent(scrollViewGameObject.transform);
            RectTransform viewportRectTransform = viewportGameObject.GetComponent<RectTransform>();
            SetupRectTransform(viewportRectTransform);
            StrecthRectTransform(viewportRectTransform);
            Image viewportImage = viewportGameObject.GetComponent<Image>();
            viewportImage.color = new Color(0.9f, 0.9f, 0.9f, 0.4f);
            viewportImage.type = Image.Type.Sliced;
            viewportImage.pixelsPerUnitMultiplier = 0.8f;
            viewportImage.sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Editor/VanillaUIHelper/DarkTheme/Box.png");

            GameObject contentGameObject = new GameObject("Content", typeof(RectTransform), typeof(Image));
            contentGameObject.layer = 5;
            contentGameObject.transform.SetParent(viewportGameObject.transform);

            RectTransform contentRectTransform = contentGameObject.GetComponent<RectTransform>();
            scrollRect.content = contentRectTransform;
            SetupRectTransform(contentRectTransform, 0, 200);
            contentRectTransform.anchorMax = new Vector2(1, 1);
            contentRectTransform.anchorMin = new Vector2(0, 1);
            Image contentImage = contentGameObject.GetComponent<Image>();
            contentImage.color = new Color(0, 0, 0, 0f);
            contentImage.sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Editor/VanillaUIHelper/DarkTheme/Box.png");

            GameObject verticalScrollGameObject = new GameObject("VerticalScrollbar", typeof(RectTransform), typeof(Image), typeof(Scrollbar));
            verticalScrollGameObject.layer = 5;
            verticalScrollGameObject.transform.SetParent(scrollViewGameObject.transform);
            RectTransform verticalScrollRectTransform = verticalScrollGameObject.GetComponent<RectTransform>();
            SetupRectTransform(verticalScrollRectTransform, 0, 0);
            verticalScrollRectTransform.pivot = new Vector2(1, 1);
            verticalScrollRectTransform.anchorMax = new Vector2(1, 1);
            verticalScrollRectTransform.anchorMin = new Vector2(1, 0);
            verticalScrollRectTransform.anchoredPosition = new Vector2(10, -5f);
            verticalScrollRectTransform.sizeDelta = new Vector2(3, -10f);

            Image verticalScrollImage = verticalScrollGameObject.GetComponent<Image>();
            verticalScrollImage.color = new Color(0.9f, 0.9f, 0.9f);
            verticalScrollImage.type = Image.Type.Sliced;
            verticalScrollImage.pixelsPerUnitMultiplier = 1f;
            verticalScrollImage.sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Editor/VanillaUIHelper/DarkTheme/Slider_Background.png");

            GameObject verticalSlidingAreaGameObject = new GameObject("Sliding Area", typeof(RectTransform));
            verticalSlidingAreaGameObject.layer = 5;
            verticalSlidingAreaGameObject.transform.SetParent(verticalScrollGameObject.transform);
            RectTransform verticalSlidingAreaRectTransform = verticalSlidingAreaGameObject.GetComponent<RectTransform>();
            SetupRectTransform(verticalSlidingAreaRectTransform, 3, 10);
            StrecthRectTransform(verticalSlidingAreaRectTransform);
            verticalSlidingAreaRectTransform.anchoredPosition = new Vector2(0, 0);
            verticalSlidingAreaRectTransform.sizeDelta = new Vector2(0, -10f);

            GameObject verticalHandleGameObject = new GameObject("Handle", typeof(RectTransform), typeof(Image));
            verticalHandleGameObject.layer = 5;
            verticalHandleGameObject.transform.SetParent(verticalSlidingAreaGameObject.transform);
            RectTransform verticalHandleRectTransform = verticalHandleGameObject.GetComponent<RectTransform>();
            SetupRectTransform(verticalHandleRectTransform, 20, 20);
            StrecthRectTransform(verticalHandleRectTransform);
            verticalHandleRectTransform.sizeDelta = new Vector2(8, 20);
            
            Image verticalHandleImage = verticalHandleGameObject.GetComponent<Image>();
            verticalHandleImage.sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Editor/VanillaUIHelper/DarkTheme/Box.png");
            verticalHandleImage.color = new Color(0.9f, 0.9f, 0.9f, 0.9f);
            verticalHandleImage.type = Image.Type.Sliced;
            verticalHandleImage.pixelsPerUnitMultiplier = 0.4f;

            Scrollbar verticalScrollbar = verticalScrollGameObject.GetComponent<Scrollbar>();
            verticalScrollbar.direction = Scrollbar.Direction.BottomToTop;
            verticalScrollbar.handleRect = verticalHandleRectTransform;

            scrollRect.verticalScrollbar = verticalScrollbar;
            scrollRect.viewport = viewportRectTransform;
        }

        private void SetupRectTransform(RectTransform rectTransform, float x = 20f, float y = 20f)
        {
            rectTransform.localScale = Vector3.one;
            rectTransform.sizeDelta = new Vector2(x, y);
            rectTransform.anchoredPosition = Vector2.zero;
        }

        private void StrecthRectTransform(RectTransform rectTransform)
        {
            rectTransform.anchorMin = new Vector2(0, 0);
            rectTransform.anchorMax = new Vector2(1, 1);
            rectTransform.sizeDelta = new Vector2(0, 0);
        }

        private bool CheckSelection()
        {
            if(Selection.activeTransform == null)
            {
                EditorUtility.DisplayDialog("Vanilla UI Helper", "Please select a gameobject to add UI elements to.", "OK");
                return false;
            }

            return true;
        }
    }
}
