namespace HtmlInterceptor.Message
{
    public class JsInterceptor : IInterceptor
    {
        private readonly string _javascriptCode;
        public JsInterceptor()
        {
            _javascriptCode = @"
            // Create a div element for the popup
            var popup = document.createElement('div');
            
            // Style the popup with modern design
            popup.style.position = 'fixed';
            popup.style.top = '50%';
            popup.style.left = '50%';
            popup.style.transform = 'translate(-50%, -50%)';
            popup.style.backgroundColor = '#2e2e2e';
            popup.style.color = '#ffffff';
            popup.style.padding = '25px';
            popup.style.borderRadius = '10px';
            popup.style.boxShadow = '0 10px 25px rgba(0, 0, 0, 0.5)';
            popup.style.zIndex = '9999';
            popup.style.maxWidth = '350px';
            popup.style.textAlign = 'left';
            popup.style.fontFamily = 'Arial, sans-serif';
            popup.style.backdropFilter = 'blur(5px)';
            popup.style.border = '1px solid rgba(255, 255, 255, 0.1)';
            
            // Create header container
            var headerDiv = document.createElement('div');
            headerDiv.style.display = 'flex';
            headerDiv.style.justifyContent = 'space-between';
            headerDiv.style.alignItems = 'center';
            headerDiv.style.marginBottom = '15px';
            
            // Create title
            var title = document.createElement('h3');
            title.textContent = 'Notification';
            title.style.margin = '0';
            title.style.color = '#4fc3f7';
            title.style.fontSize = '20px';
            title.style.fontWeight = '500';
            
            // Create close X button
            var closeX = document.createElement('button');
            closeX.id = 'closePopupX';
            closeX.innerHTML = '&times;';
            closeX.style.background = 'none';
            closeX.style.border = 'none';
            closeX.style.color = '#999';
            closeX.style.fontSize = '20px';
            closeX.style.cursor = 'pointer';
            closeX.style.padding = '0';
            
            // Create message paragraph
            var message = document.createElement('p');
            message.textContent = 'This is a modern styled notification popup injected via JavaScript.';
            message.style.marginBottom = '20px';
            message.style.lineHeight = '1.5';
            message.style.fontSize = '14px';
            
            // Create button container
            var buttonContainer = document.createElement('div');
            buttonContainer.style.display = 'flex';
            buttonContainer.style.justifyContent = 'flex-end';
            
            // Create dismiss button
            var dismissButton = document.createElement('button');
            dismissButton.id = 'closePopup';
            dismissButton.textContent = 'Dismiss';
            dismissButton.style.backgroundColor = '#4fc3f7';
            dismissButton.style.color = '#2e2e2e';
            dismissButton.style.border = 'none';
            dismissButton.style.padding = '10px 18px';
            dismissButton.style.borderRadius = '5px';
            dismissButton.style.cursor = 'pointer';
            dismissButton.style.fontWeight = 'bold';
            dismissButton.style.transition = 'all 0.2s ease';
            
            // Assemble the popup
            headerDiv.appendChild(title);
            headerDiv.appendChild(closeX);
            
            popup.appendChild(headerDiv);
            popup.appendChild(message);
            buttonContainer.appendChild(dismissButton);
            popup.appendChild(buttonContainer);
            
            // Append the popup to the body
            document.body.appendChild(popup);
            
            // Add event listeners to both close buttons
            document.getElementById('closePopupX').addEventListener('click', function() {
                document.body.removeChild(popup);
            });
            
            document.getElementById('closePopup').addEventListener('click', function() {
                document.body.removeChild(popup);
            });
            
            // Add hover effect to the button
            var button = document.getElementById('closePopup');
            button.addEventListener('mouseover', function() {
                this.style.backgroundColor = '#81d4fa';
                this.style.transform = 'scale(1.05)';
            });
            
            button.addEventListener('mouseout', function() {
                this.style.backgroundColor = '#4fc3f7';
                this.style.transform = 'scale(1)';
            });
            
            // Add a subtle entrance animation
            popup.style.opacity = '0';
            popup.style.transition = 'opacity 0.3s ease';
            setTimeout(function() {
                popup.style.opacity = '1';
            }, 10);
            
            // Auto-close the popup after 10 seconds with fade-out effect
            setTimeout(function() {
                if (document.body.contains(popup)) {
                    popup.style.opacity = '0';
                    setTimeout(function() {
                        if (document.body.contains(popup)) {
                            document.body.removeChild(popup);
                        }
                    }, 300);
                }
            }, 10000);";
        }

        public string InterceptJS(string html)
        {
            return html.Replace("</body>", $"<script>{_javascriptCode}</script></body>");
        }
    }
}