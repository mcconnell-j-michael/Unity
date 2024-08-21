public interface I_DeliveryArgumentPack
{
    I_DeliveryArgumentPack Initialize();
    void Clear();
    void SetIndex(int index);
    int GetIndex();
    void CopyInto(I_DeliveryArgumentPack pack);
}
