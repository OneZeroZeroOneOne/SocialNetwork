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
            url: 'https://16ch.ml/api/attachment',
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
                            let request = axios.post('https://16ch.ml/api/attachment/Attachment', formData, {
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
                            let request = axios.post('https://16ch.ml/api/attachment/Attachment', formData, {
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
                return {
                    abort: () => {
                        cancel()

                        abort();
                    }
                };
            },
            load: (source, load) => {
                //
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
    color: var(--attachment-drop-text-color);
}

[data-filepond-item-state*='error'] .filepond--item-panel,
[data-filepond-item-state*='invalid'] .filepond--item-panel {
    background-color: var(--attachment-drop-error);
}

.filepond--file-action-button:hover,
.filepond--file-action-button:focus {
    box-shadow: 0 0 0 0.125em var(--attachment-drop-action-button-color);
}

[data-filepond-item-state='processing-complete'] .filepond--item-panel {
    background-color: var(--attachment-drop-success);
}

.filepond--panel-root {
    border: 2px solid var(--attachment-drop-panel-border-color);
}
</style>
