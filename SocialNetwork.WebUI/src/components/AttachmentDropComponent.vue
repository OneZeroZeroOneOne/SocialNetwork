<template>
  <div id="app">
    <file-pond
            name="test"
            ref="pond"
            label-idle="Drop files here or <span class='filepond--label-action'>Browse</span>"
            allow-multiple="true"
            accepted-file-types="image/jpeg, image/png"
            v-bind:files="myFiles"
            server="http://194.99.21.140:8003/Attachment"
    />
  </div>
</template>

<script>
  // Import Vue FilePond
  import vueFilePond from 'vue-filepond'

  // Import FilePond styles
  import 'filepond/dist/filepond.min.css'

  // Import FilePond plugins
  // Please note that you need to install these plugins separately

  // Import image preview plugin styles
  import 'filepond-plugin-image-preview/dist/filepond-plugin-image-preview.min.css'

  // Import image preview and file type validation plugins
  import FilePondPluginFileValidateType from 'filepond-plugin-file-validate-type'
  import FilePondPluginImagePreview from 'filepond-plugin-image-preview'

  // Create component
  const FilePond = vueFilePond(
          FilePondPluginFileValidateType,
          FilePondPluginImagePreview
  )
  export default {
    name: 'AttachmentDropComponent',
    data: function() {
      return { myFiles: [] }
    },
    components: {
      FilePond
    }
  }
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style>
.filepond--item {
    width: calc(50% - .5em);
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
