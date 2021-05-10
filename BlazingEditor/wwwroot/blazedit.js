

window.BlazeEdit = {
    editor:null,
    configureEditor: function () {
        editor = new EditorJS(
            { 
                /** 
                 * Id of Element that should contain the Editor 
                 */ 
                holder: 'editorjs', 
              
                /** 
                 * Available Tools list. 
                 * Pass Tool's class or Settings object for each Tool you want to use 
                 */ 
                tools: { 
                  header: {
                    class: Header, 
                    inlineToolbar: ['link'] 
                  }, 
                  list: { 
                    class: List, 
                    inlineToolbar: true 
                  },
                  paragraph: {
                    class: Paragraph,
                    inlineToolbar: true,
                  },
                  embed: {
                    class: Embed,
                    config: {
                      services: {
                        youtube: true,
                        coub: true
                      }
                    }
                  },
                  inlineCode: {
                    class: InlineCode,
                    shortcut: 'CMD+SHIFT+M',
                  },
                  raw: RawTool,
                  delimiter: Delimiter,
                  image: {
                    class: ImageTool,
                    config: {
                      endpoints: {
                        byFile: '/BlogUtils/UploadImage', // Your backend file uploader endpoint
                      }
                    },
                  },
                  linkTool: {
                    class: LinkTool,
                    config: {
                      endpoint: '/BlogUtils/GetLinInfo', // Your backend endpoint for url data fetching
                    }
                  },
                  codeBox: {
                    class: CodeBox,
                    config: {
                      themeURL: 'https://cdn.jsdelivr.net/gh/highlightjs/cdn-release@9.18.1/build/styles/dracula.min.css', // Optional
                      themeName: 'atom-one-dark', // Optional
                      useDefaultTheme: 'light' // Optional. This also determines the background color of the language select drop-down
                    }
                  },
                },
              }
        );
    },
    saveEdited: function () {
      return editor.save().then((outputData) => {
        console.log('Article data: ', outputData);
        return JSON.stringify(outputData);
      });
    }
};