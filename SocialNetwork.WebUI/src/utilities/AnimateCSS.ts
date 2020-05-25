export default function animateCSS(element: Element, animation: string, prefix: string = 'animate__')
{
  // We create a Promise and return it
  return new Promise((resolve, reject) => {
    const animationName = `${prefix}${animation}`;

    element.classList.add(`${prefix}animated`, animationName);

    // When the animation ends, we clean the classes and resolve the Promise
    function handleAnimationEnd() {
        element.classList.remove(`${prefix}animated`, animationName);
        element.removeEventListener('animationend', handleAnimationEnd);

        resolve('Animation ended');
    }

    element.addEventListener('animationend', handleAnimationEnd);
  });
}

export function animateCSSCallback(element: Element, animation: string, callback: Function, prefix: string = 'animate__')
{
  // We create a Promise and return it
  return new Promise((resolve, reject) => {
    const animationName = `${prefix}${animation}`;

    element.classList.add(`${prefix}animated`, animationName);

    // When the animation ends, we clean the classes and resolve the Promise
    function handleAnimationEnd() {
        element.classList.remove(`${prefix}animated`, animationName);
        element.removeEventListener('animationend', handleAnimationEnd);

        resolve('Animation ended');
        callback();
    }

    element.addEventListener('animationend', handleAnimationEnd);
  });
}

/*export default function animateCSS(element: Element, animation_in: string, animation_out: string|undefined = undefined, prefix: string = 'animate__')
{
  // We create a Promise and return it
  return new Promise((resolve, reject) => {
    const animationInName = `${prefix}${animation_in}`;
    const animationOutName = `${prefix}${animation_out}`;

    element.classList.add(`${prefix}animated`, animationInName);

    // When the animation ends, we clean the classes and resolve the Promise
    function handleAnimationInEnd() {
        element.classList.remove(animationInName);
        element.removeEventListener('animationend', handleAnimationInEnd);
        if (animation_out !== undefined)
        {
            element.addEventListener('animationend', handleAnimationOutEnd);
            element.classList.add(animationOutName);
        }
        else
            element.classList.remove(`${prefix}animated`);

        resolve('Animation in ended');
    }

    // When the animation ends, we clean the classes and resolve the Promise
    function handleAnimationOutEnd() {
        element.classList.remove(`${prefix}animated`, animationOutName);
        element.removeEventListener('animationend', handleAnimationOutEnd);

        resolve('Animation out ended');
    }

    element.addEventListener('animationend', handleAnimationInEnd);
  });
} */