using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Unity;
using UnityEngine;

namespace BeardedManStudios.Forge.Networking.Generated
{
	[GeneratedRPC("{\"types\":[[\"int\"][\"byte[]\"][\"byte[]\"][]]")]
	[GeneratedRPCVariableNames("{\"types\":[[\"cmdIndex\"][\"commands\"][\"newWordList\"][]]")]
	public abstract partial class CommandsBehavior : NetworkBehavior
	{
		public const byte RPC_DO_COMMAND = 0 + 5;
		public const byte RPC_NEW_COMMANDS = 1 + 5;
		public const byte RPC_NEW_WORD_LIST = 2 + 5;
		public const byte RPC_COMMAND_TIMER_DONE = 3 + 5;
		
		public CommandsNetworkObject networkObject = null;

		public override void Initialize(NetworkObject obj)
		{
			// We have already initialized this object
			if (networkObject != null && networkObject.AttachedBehavior != null)
				return;
			
			networkObject = (CommandsNetworkObject)obj;
			networkObject.AttachedBehavior = this;

			base.SetupHelperRpcs(networkObject);
			networkObject.RegisterRpc("DoCommand", DoCommand, typeof(int));
			networkObject.RegisterRpc("NewCommands", NewCommands, typeof(byte[]));
			networkObject.RegisterRpc("NewWordList", NewWordList, typeof(byte[]));
			networkObject.RegisterRpc("CommandTimerDone", CommandTimerDone);

			networkObject.onDestroy += DestroyGameObject;

			if (!obj.IsOwner)
			{
				if (!skipAttachIds.ContainsKey(obj.NetworkId))
					ProcessOthers(gameObject.transform, obj.NetworkId + 1);
				else
					skipAttachIds.Remove(obj.NetworkId);
			}

			if (obj.Metadata != null)
			{
				byte transformFlags = obj.Metadata[0];

				if (transformFlags != 0)
				{
					BMSByte metadataTransform = new BMSByte();
					metadataTransform.Clone(obj.Metadata);
					metadataTransform.MoveStartIndex(1);

					if ((transformFlags & 0x01) != 0 && (transformFlags & 0x02) != 0)
					{
						MainThreadManager.Run(() =>
						{
							transform.position = ObjectMapper.Instance.Map<Vector3>(metadataTransform);
							transform.rotation = ObjectMapper.Instance.Map<Quaternion>(metadataTransform);
						});
					}
					else if ((transformFlags & 0x01) != 0)
					{
						MainThreadManager.Run(() => { transform.position = ObjectMapper.Instance.Map<Vector3>(metadataTransform); });
					}
					else if ((transformFlags & 0x02) != 0)
					{
						MainThreadManager.Run(() => { transform.rotation = ObjectMapper.Instance.Map<Quaternion>(metadataTransform); });
					}
				}
			}

			MainThreadManager.Run(() =>
			{
				NetworkStart();
				networkObject.Networker.FlushCreateActions(networkObject);
			});
		}

		protected override void CompleteRegistration()
		{
			base.CompleteRegistration();
			networkObject.ReleaseCreateBuffer();
		}

		public override void Initialize(NetWorker networker, byte[] metadata = null)
		{
			Initialize(new CommandsNetworkObject(networker, createCode: TempAttachCode, metadata: metadata));
		}

		private void DestroyGameObject(NetWorker sender)
		{
			MainThreadManager.Run(() => { try { Destroy(gameObject); } catch { } });
			networkObject.onDestroy -= DestroyGameObject;
		}

		public override NetworkObject CreateNetworkObject(NetWorker networker, int createCode, byte[] metadata = null)
		{
			return new CommandsNetworkObject(networker, this, createCode, metadata);
		}

		protected override void InitializedTransform()
		{
			networkObject.SnapInterpolations();
		}

		/// <summary>
		/// Arguments:
		/// int cmdIndex
		/// </summary>
		public abstract void DoCommand(RpcArgs args);
		/// <summary>
		/// Arguments:
		/// byte[] commands
		/// </summary>
		public abstract void NewCommands(RpcArgs args);
		/// <summary>
		/// Arguments:
		/// byte[] newWordList
		/// </summary>
		public abstract void NewWordList(RpcArgs args);
		/// <summary>
		/// Arguments:
		/// </summary>
		public abstract void CommandTimerDone(RpcArgs args);

		// DO NOT TOUCH, THIS GETS GENERATED PLEASE EXTEND THIS CLASS IF YOU WISH TO HAVE CUSTOM CODE ADDITIONS
	}
}