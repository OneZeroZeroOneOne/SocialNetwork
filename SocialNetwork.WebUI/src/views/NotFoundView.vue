<template>
    <div class="code-area">
    <span style="color: #777;font-style:italic;">
        // 404 page not found.
    </span>
    <span>
        <span style="color:#d65562;">
        if
        </span>
        (<span style="color:#4ca8ef;">!</span><span style="font-style: italic;color:#bdbdbd;">found</span>)
        {
    </span>
    <span>
        <span style="padding-left: 15px;color:#2796ec">
        <i style="width: 10px;display:inline-block"></i>throw
        </span>
        <span>
        (<span style="color: #a6a61f">"(╯°□°)╯︵ ┻━┻"</span>);
        </span>
        <span style="display:block">}</span>
        <span style="color: #777;font-style:italic;" v-if="fromBoardName!=''">
            // <a v-on:click="navigateTo">Go back to {{fromBoardName}}!</a>
        </span>
        <span style="color: #777;font-style:italic;" v-else>
            // <a v-on:click="navigateTo">Go back!</a>
        </span>
    </span>
    </div>
</template>

<script lang="ts">
import { Component, Prop, Vue } from "vue-property-decorator";

@Component({})
export default class NotFoundView extends Vue {
  @Prop() public fromBoard!: string;

  constructor() {
    super();
  }

  get fromBoardName(): string {
      return (this.fromBoard === undefined ? "" : this.fromBoard)
  }

  navigateTo() {
      this.$router.push("/" + this.fromBoardName);
  }
}
</script>


<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped lang="scss">
body {
    background: #081421;
    color: #d3d7de;
    font-family: "Courier new";
    font-size: 18px;
    line-height: 1.5em;
    cursor: default;
}

a {
	color: #fff;
    text-decoration: underline;
    cursor: pointer;
}

.code-area {
    position: absolute;
    width: 320px;
    min-width: 320px;
    top: 50%;
    left: 50%;
    -webkit-transform: translate(-50%, -50%);
    transform: translate(-50%, -50%);
}

.code-area > span {
    display: block;
}

@media screen and (max-width: 320px) {
    .code-area {
		font-size: 5vw;
		min-width: auto;
        width: 95%;
		margin: auto;
		padding: 5px;
		padding-left: 10px;
		line-height: 6.5vw;
    }
}

</style>