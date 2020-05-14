<template>
    <nav :class="classes()" role="navigation" aria-label="Menu">
        <ul class="animenu__nav">
            <li>
                <router-link to="/">Главная</router-link>
            </li>
            <li>
                <router-link to="/info">Инфо</router-link>
            </li>
            <li>
                <router-link to="/contacts">Контакты</router-link>
            </li>
        </ul>
    </nav>
</template>

<script lang="ts">
import { Component, Prop, Vue } from "vue-property-decorator";

@Component({
  components: {
  }
})
export default class NavBarComponent extends Vue {
    public scrollY: number = 0;
    public styles: any = {
        "display": "block",
        "top": "10px",
        "position": "absolute",
        "z-index": "-9999"
    }

  constructor() {
    super();
  }

  created() {
    /*window.addEventListener('scroll', (e) => {
        console.log(window.scrollY);
        this.scrollY = window.scrollY;
        console.log(this.scrollY)
        this.styles = {
            "display": "block",
            "top": this.scrollY + "px",
            "position": "absolute",
            "z-index": "-9999"
        }
    });*/
  }

  classes() {
    return {
        "animenu": true,
        //"fixed": this.scrollY > 100,
    }
  }

  /*styles() {
    return {
        "top": this.scrollY,
        "position": "absolute",
    }
  }*/
}
</script>
<style lang="scss" scoped>
// Variables
$baseMenuBackground: #192734;         // Base color theme
$secondaryMenuBackground: #0186ba;  // Secondary color (highlights, triangles...)
$gutter: 6px;                       // Base gutter
$fontSize: 12px;

// Latest CSS box model 
*, *:after, *:before {
  box-sizing: border-box; 
}

// Clear some defaults
.animenu {
  display: block;
  ul {
    padding: 0;
    list-style: none;    
    font: 0 -apple-system,
            BlinkMacSystemFont,   
            "Segoe UI",          
            "Roboto",
            "Helvetica Neue", Arial, sans-serif,
            "Apple Color Emoji", "Segoe UI Emoji", "Segoe UI Symbol";    
  }

  li, a {
    display: inline-block;
    font-size: $fontSize;
  }

  a {
    color: lighten($baseMenuBackground, 60%);
    text-decoration: none;
  }
}

// First level -> main menu items
// <nav class="animenu">
//    <ul class="animenu__nav">
//    ...
//    </ul>
//  </nav>
.animenu__nav {
  background-color: $baseMenuBackground;        

  > li {
    position: relative;
    border-right: 1px solid lighten($baseMenuBackground, 20%);

    > a {
      padding: $gutter 3 * $gutter;
      text-transform: uppercase; 
    }    

    &:hover {
      > ul {
        opacity: 1;
        visibility: visible;
        margin: 0;        
      }

      > a {
        color: #fff;
      }
    }

    // Duplicate stuff due to 
    // https://developer.microsoft.com/en-us/microsoft-edge/platform/issues/16651302/
    &:focus-within {
      > ul {
        opacity: 1;
        visibility: visible;
        margin: 0;        
      }
      
      > a {
        color: #fff;
      }
    }    
  }

  &__hasDropdown:before { 
    content:""; 
    position: absolute;
    border: 4px solid transparent;
    border-bottom: 0; 
    border-top-color: currentColor;
    top: 50%;
    margin-top: -2px;
    right: 10px;  
  }   
}

// The main breakpoint is 767px
// Adjust the first and second levels display
@media screen and (max-width: 767px) {
  .animenu__toggle {
    display: inline-block;
  }

  .animenu__nav,
  .animenu__nav__dropdown {
    display: none;
  }

  // First level -> main menu items
  // <nav class="animenu">
  //    <ul class="animenu__nav">
  //    ...
  //    </ul>
  //  </nav>    
  .animenu__nav {
    margin: $gutter 0;

    > li {
      width: 100%;
      border-right: 0;
      border-bottom: 1px solid lighten($baseMenuBackground, 25%);

      &:last-child {
        border: 0; 
      }

      &:first-child > a:after {
        content: '';
        position: absolute;
        height: 0; width: 0;
        left: 1em;
        top: -6px;
        border: 6px solid transparent;
        border-top: 0;
        border-bottom-color: inherit;
      }

      > a {
        width: 100%;
        padding: $gutter;
        border-color: $baseMenuBackground;
        position: relative; //dropdown caret
      }
    }

    a:hover {
      background-color: $secondaryMenuBackground;
      border-color: $secondaryMenuBackground;
      color: #fff;      
    }
  }
}
</style>