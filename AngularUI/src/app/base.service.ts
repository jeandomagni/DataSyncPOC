export abstract class BaseService {
    protected abstract onlineUrl: string;
    protected abstract offlineUrl: string;

    protected  getUrl() {
        if(self.navigator.onLine) {
        return this.onlineUrl;
        } else {
        return this.offlineUrl;
        }
    }
}