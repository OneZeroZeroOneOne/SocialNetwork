<template>
  <div id="app">
    <file-pond
        name="UploadFile"
        ref="pond"
        label-idle="Drop files here or <span class='filepond--label-action'>Browse</span>"
        allow-multiple="true"
        accepted-file-types="video/gif, image/gif, image/jpeg, image/png, video/mp4, video/webm"
        v-bind:files="myFiles"
        v-on:init="handleFilePondInit"
        :server="myServer"
    />
  </div>
</template>

<script>
  // Import Vue FilePond
  import vueFilePond from 'vue-filepond'
  import setOptions from 'vue-filepond'

  // Import FilePond styles
  import 'filepond/dist/filepond.min.css'

  // Import FilePond plugins
  // Please note that you need to install these plugins separately

  // Import image preview plugin styles
  import 'filepond-plugin-image-preview/dist/filepond-plugin-image-preview.min.css'

  // Import image preview and file type validation plugins
  import FilePondPluginFileValidateType from 'filepond-plugin-file-validate-type'
  import FilePondPluginImagePreview from 'filepond-plugin-image-preview'
  import axios, { AxiosStatic, AxiosResponse } from 'axios';

  // Create component
  const FilePond = vueFilePond(
    FilePondPluginFileValidateType,
    FilePondPluginImagePreview,
  )
  export default {
    name: 'AttachmentDropComponent',
    data: function() {
      return { 
        myFiles: [],
        myServer: {
            url: 'http://194.99.21.140:8003',
            process:(fieldName, file, metadata, load, error, progress, abort, transfer, options) => {
                // fieldName is the name of the input field
                // file is the actual file object to send
                const formData = new FormData();
                let width, height;
                let cancel;
                formData.append(fieldName, file, file.name);
                this.$emit('start-uploading')

                var reader = new FileReader(); // CREATE AN NEW INSTANCE.

                reader.onload = (e) => {
                    let type = e.target.result.split(';')[0];
                    type = type.split(":")[1];
                    let srcEl;
                    let source = axios.CancelToken;
                    if (type.split('/')[0] === 'image')
                    {
                        srcEl = new Image();
                        srcEl.src = e.target.result;

                        srcEl.onload = (ee) => {
                            console.log(ee)
                            width = ee.path[0].width;
                            height = ee.path[0].height;
                            let request = axios.post('http://16ch.tk/api/attachment/Attachment', formData, {
                                headers: {
                                    'Content-Type': 'multipart/form-data',
                                    'X-Width': width,
                                    'X-Height': height,
                                },
                                cancelToken: new source(function executor(c) {
                                    // An executor function receives a cancel function as a parameter
                                    cancel = c;
                                }),
                                onUploadProgress: (progressEvent) => {
                                    //const totalLength = progressEvent.lengthComputable ? progressEvent.total : progressEvent.target.getResponseHeader('content-length') || progressEvent.target.getResponseHeader('x-decompressed-content-length');
                                    progress(progressEvent.lengthComputable, progressEvent.loaded, progressEvent.total);
                                    console.log(progressEvent)
                                    /*if (totalLength !== null) {
                                        this.progressData = Math.round( (progressEvent.loaded * 100) / totalLength );
                                    }*/
                                },
                            })
                            .then(x => {
                                if (x.status === 200)
                                {
                                    load(Date.now());
                                    this.$emit('uploaded-succesfully', x)
                                } else {
                                    error(x.data.errors.UploadFile[0]);
                                    this.$emit('uploaded-error', x)
                                }
                            })
                            .catch(x => {
                                error(x);
                            })
                        }
                    } else {
                        srcEl = document.createElement('video');
                        srcEl.preload = 'metadata';
                        srcEl.onloadedmetadata = () => {
                            console.log(srcEl.videoWidth, srcEl.videoHeight)
                            let request = axios.post('http://16ch.tk/api/attachment/Attachment', formData, {
                                headers: {
                                    'Content-Type': 'multipart/form-data',
                                    'X-Width': srcEl.videoWidth,
                                    'X-Height': srcEl.videoHeight,
                                },
                                cancelToken: new source(function executor(c) {
                                    // An executor function receives a cancel function as a parameter
                                    cancel = c;
                                }),
                                onUploadProgress: (progressEvent) => {
                                    //const totalLength = progressEvent.lengthComputable ? progressEvent.total : progressEvent.target.getResponseHeader('content-length') || progressEvent.target.getResponseHeader('x-decompressed-content-length');
                                    progress(progressEvent.lengthComputable, progressEvent.loaded, progressEvent.total);
                                    /*if (totalLength !== null) {
                                        this.progressData = Math.round( (progressEvent.loaded * 100) / totalLength );
                                    }*/
                                },
                            })
                            .then(x => {
                                if (x.status === 200)
                                {
                                    load(Date.now());
                                    this.$emit('uploaded-succesfully', x)
                                } else {
                                    error(x.data.errors.UploadFile[0]);
                                    this.$emit('uploaded-error', x)
                                }
                            })
                            .catch(x => {
                                error(x);
                            })
                        }

                        srcEl.src = e.target.result
                    }
                };

                reader.readAsDataURL(file);

                /*const request = new XMLHttpRequest();
                request.open('POST', 'http://194.99.21.140:8003/Attachment');

                // Should call the progress method to update the progress to 100% before calling load
                // Setting computable to false switches the loading indicator to infinite mode
                request.upload.onprogress = (e) => {
                    progress(e.lengthComputable, e.loaded, e.total);
                };

                // Should call the load method when done and pass the returned server file id
                // this server file id is then used later on when reverting or restoring a file
                // so your server knows which file to return without exposing that info to the client
                request.onload = function() {
                    if (request.status >= 200 && request.status < 300) {
                        // the load method accepts either a string (id) or an object
                        load(request.responseText);
                        console.log(request)
                    }
                    else {
                        // Can call the error method if something is wrong, should exit after
                        error('oh no');
                    }
                };

                request.send(formData);*/
                
                
                // Should expose an abort method so the request can be cancelled
                return {
                    abort: () => {
                        // This function is entered if the user has tapped the cancel button
                        //request.abort();
                        cancel()

                        // Let FilePond know the request has been cancelled
                        abort();
                    }
                };
            },
            load: (source, load) => {
              // simulates loading a file from the server
              //console.log(source, load)
              //fetch(source).then(res => res.blob()).then(load);
            }
          } 
        }
    },
    components: {
      FilePond
    },
    methods: {
        handleFilePondInit: function() {
            //console.log(this.$refs.pond)
        }
    }
  }
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style>
.filepond--root {
    max-height: 300px;
}

.filepond--item {
    width: calc(100% - 1em);
}

.filepond--file-action-button {
    cursor: pointer;
}

.filepond--drop-label {
    color: #555;
}

[data-filepond-item-state*='error'] .filepond--item-panel,
[data-filepond-item-state*='invalid'] .filepond--item-panel {
    background-color: red;
}

.filepond--file-action-button:hover,
.filepond--file-action-button:focus {
    box-shadow: 0 0 0 0.125em rgba(172, 67, 67, 0.9);
}

[data-filepond-item-state='processing-complete'] .filepond--item-panel {
    background-color: green;
}

.filepond--panel-root {
    border: 2px solid #2c3340;
}
</style>
