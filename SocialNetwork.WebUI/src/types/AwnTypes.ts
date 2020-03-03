
type FunctionCallback = (resp: any) => any;

export declare interface VueNotification {
    success(message: string, callOptions: any): void
    info(message: string, callOptions: any): void
    async(promise: Promise<any>, onResolve: FunctionCallback | string | null, onReject: FunctionCallback | string | null, message: string | null): void
    error(message: string, callOptions: any): void
}

declare module "vue/types/vue" {
    interface Vue {
        $awn: VueNotification;
    }
}