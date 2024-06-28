public interface IRPGState
{
    /// <summary>Awakeで呼ばれるメソッド</summary>
    public void Init(BlockPlayerController player);

    /// <summary>Stateに入った時呼ばれるメソッド</summary>
    public void StartState(BlockPlayerController player);

    /// <summary>Update時、呼ばれるメソッド</summary>
    public void UpdateState(BlockPlayerController player);

    /// <summary>Stateを抜ける時呼ばれるメソッド</summary>
    public void EndState(BlockPlayerController player);
}
