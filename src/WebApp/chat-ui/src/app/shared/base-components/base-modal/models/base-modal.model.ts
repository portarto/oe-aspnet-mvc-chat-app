export interface BaseModalOptions<TData = any> {
  title: string;
  label: string;
  cancelText: string;
  okText: string;
  data?: TData;
}
