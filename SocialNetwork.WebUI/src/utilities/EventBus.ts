class EventBus {
    public mainApp!: Vue;

    setupMainApp() {
      let app: any = document.getElementById("app");
      this.mainApp = app.__vue__
    }

    public emit(eventName: string, ...props: any[]): void {
        if (this.mainApp === undefined || this.mainApp === null)
            this.setupMainApp()
            
        this.mainApp.$emit(eventName, ...props)
    }

    public subscribe(eventName: string, callback: any): Vue {
        if (this.mainApp === undefined || this.mainApp === null)
            this.setupMainApp()
        
        return this.mainApp.$on(eventName, callback);
    }

    public unsubscribe(eventName: string, callback: any): Vue {
        if (this.mainApp === undefined || this.mainApp === null)
            this.setupMainApp()
        
        return this.mainApp.$off(eventName, callback);
    }
}

export default new EventBus();