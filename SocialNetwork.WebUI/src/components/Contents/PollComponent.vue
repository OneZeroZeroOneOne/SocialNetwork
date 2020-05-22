<template>
  <div class="poll-container">
    <div class="poll-voter">
      <h4 class="poll-title">{{pollTitle}}</h4>
      <hr style="border: none; height: 2px; background: #ADC0DF;" />
      <table class="poll-list">
        <tbody>
          <tr class="vote-row" v-for="variant in pollVariants" :key="variant.id">
            <td class="poll-option">
              <p>{{variant.name}}</p>
              <div class="poll-option-bar" :style="variantPct(variant)" />
            </td>
            <td>
              <button
                class="vote-btn"
                v-on:click="userChoose(variant)"
                :style="buttonStyle(variant)"
              >
                {{userChoosed === false ? 'vote' : '' }}
                <div v-if="userChoosed === true">
                  <font-awesome-icon :icon="userChoosenId === variant.id ? 'check' : 'times'" />
                </div>
              </button>
            </td>
          </tr>
        </tbody>
      </table>
    </div>
  </div>
</template>

<script lang="ts">
import { Component, Prop, Vue, Watch } from "vue-property-decorator";

@Component({
  components: {}
})
export default class PollComponent extends Vue {
  public pollTitle: string = "Select one pls";

  public userChoosed: boolean = false;
  public userChoosenId: number = 0;

  public pollVariants: object = [
    { name: "Panda", votes: 0, pct: 80, id: 0 },
    { name: "Iguana", votes: 0, pct: 5, id: 1 },
    { name: "Grizzly Bear", votes: 40, pct: 0, id: 2 },
    { name: "Ostrich", votes: 0, pct: 60, id: 3 },
    { name: "Leopard", votes: 0, pct: 10, id: 4 },
    { name: "Fox", votes: 0, pct: 50, id: 5 }
  ];

  constructor() {
    super();
  }
  //;

  buttonStyle(variant) {
    let style = {
      "min-width": "40px",
      display: "flex",
      "align-content": "center",
      "justify-content": "center"
    };

    if (this.userChoosed) {
      style["border-radius"] = "20%";
      style["min-width"] = "30px";
      style["width"] = "30px";
    }
    return style;
  }

  variantPct(variant) {
    if (this.userChoosed)
      if (this.userChoosenId === variant.id)
        return {
          width: variant.pct + "%"
        };
      else
        return {
          width: variant.pct + "%"
        };
    return "";
  }

  userChoose(variant) {
    console.log(variant);
    this.userChoosenId = variant.id;
    this.userChoosed = true;
  }
}
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped lang="scss">
@import url("https://fonts.googleapis.com/css?family=Nunito|Bungee");

* {
  box-sizing: border-box;
  font-family: "Nunito";
  margin: 0;
  padding: 0;
}
body {
  padding: 10px;
}
.poll-container {
  margin-top: 100px;
  width: 100%;
  height: auto;
  position: relative;
  border: 3px solid #adc0df;
  border-radius: 3px;
  padding: 10px;
  box-sizing: border-box;
}
.poll-division .bar:last-child {
  border-right: none;
}
.poll-voter {
  width: 100%;
  height: auto;
  position: relative;
  padding: 10px;
  border-radius: 3px;
  background: #eee;
}
.poll-voter .poll-title {
  margin-bottom: 5px;
  color: #393939;
  margin-left: 10px;
  font-family: "Bungee";
  color: #1e417c;
}

.poll-list {
  width: 100%;
  position: relative;
  margin-top: 10px;
  border-spacing: 7px 7px;
}

.poll-option {
  background: #fafafa;
  height: auto;
  border-radius: 3px;
  width: 100%;
  padding: 7px 14px;
  padding-left: 10px;
  position: relative;
  z-index: 5;
  min-height: 40px;
  overflow: hidden;
}
.poll-option p {
  z-index: 20;
}

.poll-option-bar {
  width: 0%;
  position: absolute;
  left: 0px;
  top: 0px;
  height: 100%;
  background: #dce6f7;
  z-index: -10;
  transition: 1s ease-out;
  transform: scale(1);
}
.vote-btn {
  //border: 2px solid #256b33;
  border: none;
  border-radius: 3px;
  padding: 7px 14px;
  background: #39a84f;
  color: white;
  float: right;
  cursor: pointer;
  outline: none;
  transition: 250ms;
}
.vote-btn:active {
  background: #63ce78;
  border: 0px;
  transition: 0ms;
}

.checkup {
  font-weight: bold;
  animation: popin 350ms linear;
}

@keyframes popin {
  0% {
    transform: scale(0);
  }
  65% {
    transform: scale(5);
  }
  80% {
    transform: scale(0);
  }
  100% {
    transform: scale(1);
  }
}
</style>
