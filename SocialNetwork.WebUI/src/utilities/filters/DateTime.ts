import { VueConstructor } from 'vue/types/umd';

function formatDate(value: string) {
  if (value) {
    const options = {
      year: "numeric",
      month: "long",
      day: "numeric",
      hour: "numeric",
      minute: "numeric",
      weekday: "long",
    };
    return new Date(value).toLocaleDateString(undefined, options);
  }
}

export default function registerFilter(vue: VueConstructor<Vue>) {
    vue.filter("formatDate", formatDate);
}