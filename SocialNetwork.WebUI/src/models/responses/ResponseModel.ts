import { ResponseState } from '../enum/ResponseState';

export class ResponseModel<T> {
    state: ResponseState;
    value: T;

    /*constructor(state: ResponseState, value: T){
        this.state = state;
        this.value = value;
    }*/
}