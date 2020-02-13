<template>
  <div id="app">
    <div id="nav"></div>
    <router-view/>
    <notifications group="foo" position="bottom right"/>
  </div>
</template>

<style lang="scss">
html {
  background: url('assets/background-tiled-memphis.png');
}
</style>
